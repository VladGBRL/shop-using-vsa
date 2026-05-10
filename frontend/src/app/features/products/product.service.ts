import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { ProductDto } from './product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private http = inject(HttpClient);
  private apiUrl = '/api/products';

  private platformId = inject(PLATFORM_ID);

  getProducts(): Observable<ProductDto[]> {
    if (isPlatformBrowser(this.platformId)) {
      return this.http.get<ProductDto[]>(this.apiUrl);
    }
    return of([]);
  }
}
