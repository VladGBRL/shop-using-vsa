import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductService } from '../product.service';
import { CartService } from '../../../features/cart/cart.service';
import { ProductDto } from '../product.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-list-component.html',
  styleUrls: ['./product-list-component.css']
})
export class ProductListComponent implements OnInit {
  private productService = inject(ProductService);
  private cartService = inject(CartService);

  
  products = signal<ProductDto[]>([]);
  isLoading = signal<boolean>(true);
  error = signal<string | null>(null);

  ngOnInit(): void {
    this.fetchProducts();
  }

  fetchProducts(): void {
    this.isLoading.set(true);
    this.productService.getProducts().subscribe({
      next: (data) => {
        this.products.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to fetch products', err);
        this.error.set('Failed to load products. Ensure the backend is running.');
        this.isLoading.set(false);
      }
    });
  }

  addToCart(product: ProductDto): void {
    this.cartService.addItem(product.id, 1).subscribe();
  }
}
