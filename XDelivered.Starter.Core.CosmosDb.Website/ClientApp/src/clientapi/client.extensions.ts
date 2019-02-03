import { AuthService } from './../app/services/authService';

export class BaseClient {
  protected transformOptions(options: RequestInit) {
    if (AuthService.isLoggedIn()) {
      const token = 'Bearer ' + AuthService.Token.token;

      options.headers = {
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: token
      };
    }
    return Promise.resolve(options);
  }
}
