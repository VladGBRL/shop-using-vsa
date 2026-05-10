import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartService } from '../cart.service';
import { CartItem } from '../cart.model';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart-component.html',
  styleUrls: ['./cart-component.css']
})
export class CartComponent implements OnInit {
  private cartService = inject(CartService);

  items: CartItem[] = [];
  isLoading = true;

  get total(): number {
    return this.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
  }

  ngOnInit(): void {
    this.cartService.loadCart().subscribe({
      next: (items: CartItem[]) => {
        this.items = items;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });

    this.cartService.cartItems$.subscribe((items: CartItem[]) => {
      this.items = items;
    });
  }

  removeItem(productId: number): void {
    this.cartService.removeItem(productId).subscribe();
  }
}
