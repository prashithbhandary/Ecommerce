export interface Order {
    id: number;
    status: string;
    totalAmount: number;
    address: string;
    paymentMethod: string;
    transactionId: string;
    items: OrderItem[];
  }  
  
  export interface OrderDetail {
    id: number;
    status: string;
    totalAmount: number;
    address: string;
    paymentMethod: string;
    transactionId: string;
    items: OrderItem[];
  }
  
  export interface OrderItem {
    productName: string;
    price: number;
    quantity: number;
  }

  export interface AdminOrder {
    id: number;
    status: string;
    totalAmount: number;
    paymentMethod: string;
    transactionId: string;
    customerName: string;
    customerEmail: string;
    address: string;
  }
  
  export interface AdminOrderDetail extends AdminOrder {
    items: OrderItem[];
  }
  
  