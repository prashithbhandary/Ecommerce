import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CartItem } from '../../models/cart-item.model';
import { Observable, take } from 'rxjs';
import { CartService } from '../../services/cart.service';
import { AccountService } from '../../services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Address } from '../../models/address.model';
import { User } from '../../models/user.model';
import bootstrap from 'bootstrap';
import { isPlatformBrowser } from '@angular/common';
import { PaymentService } from '../../services/payment.service';
import { Router } from '@angular/router';

declare var Razorpay: any;

@Component({
  selector: 'app-checkout',
  standalone: false,
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent implements OnInit {
  checkoutForm!: FormGroup;
  cartItems$!: Observable<CartItem[]>;
  totalAmount$!: Observable<number>;
  shippingAddress?: Address;
  user?: User;
  token: any;
  paymentMethodControl = new FormControl('cod', Validators.required);
  userAddresses: Address[] = [];

  constructor(
    private fb: FormBuilder,
    private cartService: CartService,
    private accountService: AccountService,
    private toastr: ToastrService,
    private paymentService: PaymentService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}

  ngOnInit(): void {
    this.cartItems$ = this.cartService.getCartItems();
  this.totalAmount$ = this.cartService.getTotalAmount();

  this.checkoutForm = this.fb.group({
    paymentMethod: this.paymentMethodControl
  });
  
  this.token = this.accountService.getToken();
  this.loadUserInfo();
  this.loadPrimaryAddress();
  }

  loadPrimaryAddress(): void {
    
    if (!this.token) return;

    const decoded: any = this.accountService.decodeToken(this.token);
    if (!decoded || !decoded.sub) {
      return;
    }

    const userId = parseInt(decoded.sub);
    this.accountService.getUserAddresses(userId).subscribe({
      next: (addresses) => {
        const primary = addresses.find(a => a.isPrimary);
        if (primary) {
          this.shippingAddress = primary;
        } else {
          this.toastr.warning('No primary address found');
        }
      },
      error: (err) => {
        this.toastr.error('Failed to load address');
        console.error(err);
      }
    });
  }

  openAddressModal(): void {
    const decoded: any = this.accountService.decodeToken(this.token);
    if (!decoded?.sub) return;

    const userId = parseInt(decoded.sub);
    this.accountService.getUserAddresses(userId).subscribe({
      next: (addresses) => {
        this.userAddresses = addresses;

        // ✅ Only run this in the browser
        if (isPlatformBrowser(this.platformId)) {
          const modalEl = document.getElementById('addressModal');
          if (modalEl) {
            // Dynamically import Bootstrap only in browser
            import('bootstrap').then((bootstrap) => {
              const modal = new bootstrap.Modal(modalEl);
              modal.show();
            });
          }
        }
      },
      error: () => this.toastr.error('Failed to load addresses')
    });
  }
  
  selectAddress(address: Address): void {
    this.shippingAddress = address;
    this.toastr.success('Shipping address updated');
  }

  loadUserInfo(): void {
    const decoded: any = this.accountService.decodeToken(this.token);
    if (!decoded || !decoded.sub) {
      return;
    }
  
    const userId = parseInt(decoded.sub);
    this.accountService.getUserById(userId).subscribe({
      next: (user) => {
        this.user = user;
      },
      error: (err) => {
        this.toastr.error('Failed to load user info');
        console.error(err);
      }
    });
  }

  onSubmit(): void {
    
    if (this.checkoutForm.invalid || !this.shippingAddress || !this.user) {
      this.toastr.error('Please complete the form');
      return;
    }
  
    if (this.checkoutForm.value.paymentMethod === 'cod') {
      this.placeCodOrder();
    } else {
      this.startRazorpayPayment();
    }
  }

  placeCodOrder(): void {
    this.totalAmount$.pipe(take(1)).subscribe(amount => {
      if (amount) {
        this.cartItems$.pipe(take(1)).subscribe(cartItems => {
          this.paymentService.placeCodOrder(amount, this.shippingAddress!.id, cartItems).subscribe({
            next: () => {
              this.toastr.success('Order placed with Cash on Delivery');
              // Optional: Redirect user to Order Success page
            },
            error: (err) => {
              this.toastr.error('Failed to place COD order');
              console.error(err);
            }
          });
        });
      }
    });
  }  

  startRazorpayPayment(): void {
  this.totalAmount$.pipe(take(1)).subscribe(amount => {
    if (amount) {
      this.cartItems$.pipe(take(1)).subscribe(cartItems => {
        this.paymentService.createOrder(amount, this.shippingAddress!.id, cartItems).subscribe({
          next: (res) => {
            this.openRazorpayCheckout(res.orderId, amount);
          },
          error: (err) => {
            this.toastr.error('Failed to create Razorpay order');
            console.error(err);
          }
        });
      });
    }
  });
}

  openRazorpayCheckout(orderId: string, amount: number): void {
    
    const options: any = {
      key: 'rzp_test_3tg6ekhj4lMG3l', // Replace with your Razorpay key_id
      amount: amount,
      currency: 'INR',
      name: 'Clothing Store',
      description: 'Purchase Products',
      order_id: orderId,
      handler: (response: any) => {
        this.verifyPayment(response);
      },
      prefill: {
        name: this.user?.fullName,
        email: this.user?.email,
        contact: this.user?.phoneNumber
      },
      notes: {
        address: `${this.shippingAddress?.addressLine1}, ${this.shippingAddress?.city}`
      },
      theme: {
        color: '#212529'
      }
    };

    const rzp = new Razorpay(options);
    rzp.open();
  }

  verifyPayment(response: any): void {
    const verifyData = {
      orderId: response.razorpay_order_id,
      paymentId: response.razorpay_payment_id,
      signature: response.razorpay_signature
    };

    this.paymentService.verifyPayment(verifyData).subscribe({
      next: () => {
        this.toastr.success('Payment verified and order placed successfully');
        // Optionally: Clear the cart after successful payment
        this.sendOrderConfirmationEmail();
      },
      error: (err) => {
        this.toastr.error('Payment verification failed');
        console.error(err);
      }
    });
  }

  sendOrderConfirmationEmail(): void {
    this.totalAmount$.pipe(take(1)).subscribe(totalAmount => {
      if (totalAmount !== undefined) {
        this.cartItems$.pipe(take(1)).subscribe(cartItems => {
  
          const emailRequest = {
            toEmail: this.user?.email,
            fullName: this.user?.fullName,
            shippingAddress: this.shippingAddress,
            cartItems: cartItems,
            totalAmount: totalAmount
          };
  
          this.cartService.clearCart();
  
          this.paymentService.sendOrderConfirmationEmail(emailRequest).subscribe({
            next: () => {
              this.router.navigate(['/']); 
            },
            error: (err) => {
              console.error(err);
              this.toastr.error('Failed to send order email');
            }
          });
        });
      } else {
        this.toastr.error('Total amount not available');
      }
    });
  }
  
  
}
