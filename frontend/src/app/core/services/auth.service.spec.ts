import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { PLATFORM_ID } from '@angular/core';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        AuthService,
        { provide: PLATFORM_ID, useValue: 'browser' }
      ]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
    
    
    localStorage.clear();
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should login and store token', () => {
    const mockResponse = { token: 'fake-jwt-token' };
    
    service.login({ email: 'test@user.com', password: 'password123' }).subscribe(response => {
      expect(response.token).toBe('fake-jwt-token');
      expect(localStorage.getItem('token')).toBe('fake-jwt-token');
      expect(service.isLoggedIn()).toBe(true);
    });

    const req = httpMock.expectOne('/api/auth/login');
    expect(req.request.method).toBe('POST');
    req.flush(mockResponse);
  });

  it('should logout and clear token', () => {
    localStorage.setItem('token', 'some-token');
    service.logout();
    expect(localStorage.getItem('token')).toBeNull();
    expect(service.isLoggedIn()).toBe(false);
  });
});
