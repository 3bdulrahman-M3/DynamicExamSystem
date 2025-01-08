import { Injectable, Inject, inject } from '@angular/core';
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
  HttpHandlerFn,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthenticationService } from './services/auth.service';

export function loggingInterceptor(
  req: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<any>> {
  const authToken = inject(AuthenticationService);
  const token = authToken.getToken();


  if (token) {
    const cloned = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`,
      },
    });
    console.log(cloned);
    return next(cloned);
  }

  return next(req);
}
