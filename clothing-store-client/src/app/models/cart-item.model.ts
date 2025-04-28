import { Product } from "./product.model";
import { ProductVariant } from "./ProductVariant.model";

export interface CartItem {
  product: Product;
  quantity: number;
  variant?: ProductVariant; 
}

