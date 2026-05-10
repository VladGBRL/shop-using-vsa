import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CartService } from './cart.service';
import { PLATFORM_ID } from '@angular/core';

describe('CartService', () => {
  let service: CartService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        CartService,
        { provide: PLATFORM_ID, useValue: 'browser' }
      ]
    });
    service = TestBed.inject(CartService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should load cart items', () => {
    const mockItems = [{ productId: 1, name: 'Test Product', price: 10, quantity: 2 }];
    
    service.loadCart().subscribe(items => {
      expect(items.length).toBe(1);
      expect(items[0].name).toBe('Test Product');
    });

    const req = httpMock.expectOne('/api/cart');
    req.flush(mockItems);
    
    
    service.cartItems$.subscribe(items => {
      expect(items.length).toBe(1);
    });
  });

  it('should update cart count reactively', () => {
    const mockItems = [{ productId: 1, quantity: 1 }, { productId: 2, quantity: 3 }];
    let currentCount = 0;
    
    service.cartCount$.subscribe(count => currentCount = count);

    service.loadCart().subscribe();
    const req = httpMock.expectOne('/api/cart');
    req.flush(mockItems);

    expect(currentCount).toBe(2);
  });
});
