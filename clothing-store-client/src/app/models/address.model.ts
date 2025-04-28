export interface Address {
    id?: number;
    name:string;
    addressLine1: string;
    addressLine2?: string;
    city: string;
    state: string;
    postalCode: string;
    country: string;
    isPrimary: boolean;
  }
  