// src/app/models/product.model.ts (create or update this file)

export interface ProductImage {
  imageUrl: string;
  isMain: boolean;
  order: number;
}

export interface ProductVariant {
  id: number;
  size: string;
  color: string;
  stock: number;
  sku: string;
}

export interface Product {
  id: number;
  name: string;
  description: string;
  price: number;
  stock: number;
  rating?: number;
  categoryName: string;
  brandName: string;
  images: ProductImage[];
  variants: ProductVariant[];
}
