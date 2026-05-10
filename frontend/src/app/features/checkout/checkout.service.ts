import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, timeout, catchError, throwError } from 'rxjs';

export interface CheckoutRequest {
  shippingAddress: string;
}

export interface CheckoutResponse {
  orderId: number;
  totalPrice: number;
  message: string;
}

@Injectable({ providedIn: 'root' })
export class CheckoutService {
  private http = inject(HttpClient);

  placeOrder(request: CheckoutRequest): Observable<CheckoutResponse> {
    return this.http.post<CheckoutResponse>('/api/checkout', request).pipe(
      timeout(10000),
      catchError(err => {
        return throwError(() => err);
      })
    );
  }
}
