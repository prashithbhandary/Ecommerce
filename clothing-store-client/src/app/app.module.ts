import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { HomeComponent } from './components/home/home.component';
import { ProductsComponent } from './components/products/products.component';
import { ProductCardComponent } from './components/product-card/product-card.component';
import { CartComponent } from './components/cart/cart.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';
import { ProductDetailsComponent } from './components/product-details/product-details.component';
import { CategoryFilterComponent } from './components/category-filter/category-filter.component';
import { SearchComponent } from './components/search/search.component';
import { CategoryListComponent } from './admin/category-list/category-list.component';
import { CategoryFormComponent } from './admin/category-form/category-form.component';
import { TruncatePipe } from './pipe/truncate.pipe';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { SharedModule } from './Shared/SharedModule';
import { RouterModule } from '@angular/router';
import { CheckoutComponent } from './components/checkout/checkout.component';
import { AddressListComponent } from './components/address/address-list/address-list.component';
import { AddressFormComponent } from './components/address/address-form/address-form.component';
import { ProfileComponent } from './components/profile/profile.component';
import { OrdersComponent } from './components/orders/orders.component';
import { OrderDetailsComponent } from './components/order-details/order-details.component';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    ProductsComponent,
    ProductCardComponent,
    CartComponent,
    LoginComponent,
    RegisterComponent,
    ProductDetailsComponent,
    CategoryFilterComponent,
    SearchComponent,
    CategoryListComponent,
    CategoryFormComponent,
    MainLayoutComponent,
    CheckoutComponent,
    AddressListComponent,
    AddressFormComponent,
    ProfileComponent,
    OrdersComponent,
    OrderDetailsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    RouterModule,
    BrowserAnimationsModule,
    SharedModule,
    ToastrModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }