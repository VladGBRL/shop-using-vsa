import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cartItemsSignal = signal<any[]>([]);

  addToCart(product: any) {
    this.cartItemsSignal.update(items => [...items, product]);
    console.log(`Added ${product.name} to cart. Total items: ${this.cartItemsSignal().length}`);
  }
}
