import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { CartService } from '../../features/cart/cart.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  private cartService = inject(CartService);
  private authService = inject(AuthService);
  private router = inject(Router);

  isMenuCollapsed = true;
  cartCount$ = this.cartService.cartCount$;

  toggleMenu(): void {
    this.isMenuCollapsed = !this.isMenuCollapsed;
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
