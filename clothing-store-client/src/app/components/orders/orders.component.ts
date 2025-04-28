import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { ToastrService } from 'ngx-toastr';
import { Order } from '../../models/Order.model';

@Component({
  selector: 'app-orders',
  standalone: false,
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  selectedOrder: Order | null = null;

  constructor(private orderService: OrderService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.orderService.getOrders().subscribe(
      (orders) => {
        console.log('Orders:', orders);
        this.orders = orders;
      },
      (error) => {
        this.toastr.error('Failed to load orders');
      }
    );
  }

  viewOrderDetails(orderId: number): void {
    this.orderService.getOrderDetails(orderId).subscribe(
      (orderDetails) => {
        console.log('Order details:', orderDetails);
        this.selectedOrder = orderDetails;
      },
      (error) => {
        this.toastr.error('Failed to load order details');
      }
    );
  }
}
