import { Component, OnInit } from '@angular/core';
import { Address } from '../../../models/address.model';
import { AccountService } from '../../../services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-address-list',
  standalone: false,
  templateUrl: './address-list.component.html',
  styleUrl: './address-list.component.scss'
})
export class AddressListComponent implements OnInit {
  addresses: Address[] = [];

  constructor(private accountService: AccountService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadAddresses();
  }

  loadAddresses(): void {
    const token = this.accountService.getToken();
    if (!token) return;
  
    const decoded: any = this.accountService.decodeToken(token);
    if (!decoded || !decoded.sub) {
      this.toastr.error('User ID not found in token');
      return;
    }
  
    const userId = parseInt(decoded.sub);
    this.accountService.getUserAddresses(userId).subscribe({
      next: (addresses) => {
        this.addresses = addresses;
      },
      error: (err) => {
        this.toastr.error('Failed to load addresses');
        console.error(err);
      }
    });
  }  

  deleteAddress(id: number): void {
    this.accountService.deleteAddress(id).subscribe(() => {
      this.loadAddresses();
    });
  }
}
