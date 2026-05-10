import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, map, Observable, of, tap } from 'rxjs';
import { CartItem } from './cart.model';

@Injectable({ providedIn: 'root' })
export class CartService {
  private http = inject(HttpClient);
  private apiUrl = '/api/cart';

  private platformId = inject(PLATFORM_ID);
  private _cartItems$ = new BehaviorSubject<CartItem[]>([]);

  
  cartItems$ = this._cartItems$.asObservable();

  
  cartCount$ = this._cartItems$.pipe(map(items => items.length));

  loadCart(): Observable<any> {
    if (isPlatformBrowser(this.platformId)) {
      return this.http.get<CartItem[]>(this.apiUrl).pipe(
        tap(items => this._cartItems$.next(items))
      );
    }
    return of([]);
  }

  addItem(productId: number, quantity: number): Observable<any> {
    if (isPlatformBrowser(this.platformId)) {
      return this.http.post(this.apiUrl, { productId, quantity }).pipe(
        tap(() => this.loadCart().subscribe())
      );
    }
    return of(null);
  }

  removeItem(productId: number): Observable<any> {
    if (isPlatformBrowser(this.platformId)) {
      return this.http.delete(`${this.apiUrl}/${productId}`).pipe(
        tap(() => {
          
          const updated = this._cartItems$.value.filter(i => i.productId !== productId);
          this._cartItems$.next(updated);
        })
      );
    }
    return of(null);
  }
}
