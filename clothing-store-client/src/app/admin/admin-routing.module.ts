import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from '../guards/admin.guard';
import { CategoryListComponent } from './category-list/category-list.component';
import { CategoryFormComponent } from './category-form/category-form.component';
import { AdminComponent } from './admin.component';
import { UserFormComponent } from './user/user-form/user-form.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { ProductListComponent } from './product/product-list/product-list.component';
import { BrandListComponent } from './brand/brand-list/brand-list.component';
import { BrandFormComponent } from './brand/brand-form/brand-form.component';
import { ProductFormComponent } from './product/product-form/product-form.component';
import { AdminOrderComponent } from './admin-order/admin-order.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [AdminGuard],
    children: [
      { path: '', redirectTo: 'categories', pathMatch: 'full' },

      // Category Routes
      { path: 'categories', component: CategoryListComponent },
      { path: 'categories/new', component: CategoryFormComponent },
      { path: 'categories/edit/:id', component: CategoryFormComponent },

      // Brand Routes
      { path: 'brands', component: BrandListComponent },
      { path: 'brands/new', component: BrandFormComponent },
      { path: 'brands/edit/:id', component: BrandFormComponent },

      // User Routes
      { path: 'users', component: UserListComponent },
      { path: 'users/new', component: UserFormComponent },
      { path: 'users/edit/:email', component: UserFormComponent },

      // Product Routes
      { path: 'products', component: ProductListComponent },
      { path: 'products/add', component: ProductFormComponent },
      { path: 'products/edit/:id', component: ProductFormComponent },

      { path: 'orders', component: AdminOrderComponent },
    ]
  }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }