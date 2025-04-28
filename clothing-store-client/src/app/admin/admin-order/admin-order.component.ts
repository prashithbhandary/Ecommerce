import { Component } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { AdminOrder, AdminOrderDetail } from '../../models/Order.model';

@Component({
  selector: 'app-admin-order',
  standalone: false,
  templateUrl: './admin-order.component.html',
  styleUrl: './admin-order.component.scss'
})
export class AdminOrderComponent {
  adminOrders: AdminOrder[] = [];
  selectedOrder: AdminOrderDetail | null = null;
  selectedStatus: string = '';

  constructor(private adminOrderService: OrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.adminOrderService.getOrdersAdmin(this.selectedStatus).subscribe({
      next: (orders) => {
        this.adminOrders = orders;
      },
      error: (err) => {
        console.error('Failed to load orders', err);
      }
    });
  }

  viewOrderDetails(orderId: number): void {
    this.adminOrderService.getOrderDetailsAdmin(orderId).subscribe({
      next: (orderDetail) => {
        this.selectedOrder = orderDetail;
      },
      error: (err) => {
        console.error('Failed to load order details', err);
      }
    });
  }

  filterOrders(): void {
    this.loadOrders();
  }

  closeModal(): void {
    this.selectedOrder = null;
  }
}
