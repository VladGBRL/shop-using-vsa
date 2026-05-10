import { Routes } from '@angular/router';
import { ProductListComponent } from './features/products/product-list/product-list-component';
import { LoginComponent } from './features/auth/login/login-component';
import { RegisterUserComponent } from './features/auth/register/register-user-component';
import { CartComponent } from './features/cart/cart-component/cart-component';
import { CheckoutComponent } from './features/checkout/checkout.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {path:'', component: ProductListComponent},
    {path:'products', component: ProductListComponent},
    {path:'login', component: LoginComponent},
    {path:'register', component: RegisterUserComponent},
    {path:'cart', component: CartComponent},
    {path:'checkout', component: CheckoutComponent, canActivate: [authGuard]},
    {path:'**', redirectTo: '', pathMatch: 'full'}
];
