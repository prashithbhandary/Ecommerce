import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminComponent } from './admin.component';
import { AdminNavbarComponent } from './admin-navbar/admin-navbar.component';
import { UserFormComponent } from './user/user-form/user-form.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { UserListComponent } from './user/user-list/user-list.component';
import { ProductListComponent } from './product/product-list/product-list.component';
import { BrandListComponent } from './brand/brand-list/brand-list.component';
import { BrandFormComponent } from './brand/brand-form/brand-form.component';
import { ProductFormComponent } from './product/product-form/product-form.component';
import { TruncatePipe } from '../pipe/truncate.pipe';
import { SharedModule } from '../Shared/SharedModule';
import { AdminOrderComponent } from './admin-order/admin-order.component';


@NgModule({
  declarations: [
    AdminComponent,
    AdminNavbarComponent,
    UserFormComponent,
    UserListComponent,
    ProductListComponent,
    BrandListComponent,
    BrandFormComponent,
    ProductFormComponent,
    AdminOrderComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule
  ]
})
export class AdminModule { }
