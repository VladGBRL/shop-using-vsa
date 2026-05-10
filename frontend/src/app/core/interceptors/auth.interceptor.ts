import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private platformId = inject(PLATFORM_ID);

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    let token = null;
    if (isPlatformBrowser(this.platformId)) {
      token = localStorage.getItem('token');
    }

    if (token) {
      const cloned = req.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      });
      return next.handle(cloned);
    }
    return next.handle(req);
  }
}
