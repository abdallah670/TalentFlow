import { ErrorHandler, Injectable, Injector } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class GlobalErrorHandler implements ErrorHandler {
  constructor(private injector: Injector) {}

  handleError(error: any): void {
    let message = 'An unexpected error occurred. Please try again later.';

    if (error instanceof HttpErrorResponse) {
      if (!navigator.onLine) {
        message = 'No Internet Connection. Please check your network.';
      } else {
        message = error.error?.message || `Server returned code: ${error.status}`;
      }
    } else {
      console.error('Client Error:', error);
      message = error.message ? error.message : error.toString();
    }

    try {
      const snackBar = this.injector.get(MatSnackBar);
      snackBar.open(message, 'Close', { duration: 5000, panelClass: ['error-snackbar'] });
    } catch (e) {
      console.error('GlobalErrorHandler caught:', error);
    }
  }
}
