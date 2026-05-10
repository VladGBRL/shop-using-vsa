import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CheckoutService } from './checkout.service';
import { CartService } from '../cart/cart.service';
import { CartItem } from '../cart/cart.model';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  private fb = inject(FormBuilder);
  private checkoutService = inject(CheckoutService);
  private cartService = inject(CartService);
  private router = inject(Router);

  checkoutForm!: FormGroup;
  isSubmitting = false;
  errorMessage: string | null = null;
  successMessage: string | null = null;
  
  cartItems$ = this.cartService.cartItems$;
  cartTotal = 0;

  ngOnInit(): void {
    this.checkoutForm = this.fb.group({
      shippingAddress: ['', [Validators.required, Validators.minLength(5)]]
    });

    this.cartItems$.subscribe((items: CartItem[]) => {
      this.cartTotal = items.reduce((sum: number, item: CartItem) => sum + (item.price * item.quantity), 0);
    });
    
    
    this.cartService.loadCart().subscribe();
  }

  onSubmit(): void {
    if (this.checkoutForm.invalid) {
      this.checkoutForm.markAllAsTouched();
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = null;

    this.checkoutService.placeOrder({
      shippingAddress: this.checkoutForm.value.shippingAddress
    }).subscribe({
      next: (response) => {
        this.isSubmitting = false;
        this.successMessage = `Order #${response.orderId} placed successfully! Total: $${response.totalPrice.toFixed(2)}`;
        
        
        this.cartService.loadCart().subscribe();
        
        setTimeout(() => {
          this.router.navigate(['/products']);
        }, 3000);
      },
      error: (err) => {
        this.isSubmitting = false;
        console.error('FULL ERROR OBJECT:', err);
        
        if (err.name === 'TimeoutError') {
          this.errorMessage = 'The server is not responding. Please check if your SQL Database is running.';
        } else if (err.status === 401) {
          this.errorMessage = 'Your session has expired. Please log in again.';
        } else if (err.status === 400) {
          this.errorMessage = err.error?.message || err.error?.Message || 'Invalid order data. Is your cart empty?';
        } else {
          this.errorMessage = `Error (${err.status}): ${err.error?.message || err.message || 'Connection failed'}`;
        }
      }
    });
  }
}
