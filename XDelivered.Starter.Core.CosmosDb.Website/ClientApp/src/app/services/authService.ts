import { UserInfoResponseModel, OperationResultOfLoginResponse } from './../../clientapi/client';
import { Injectable } from '@angular/core';
import { Client, LoginRequestModel, LoginResponse } from '../../clientapi/client';

@Injectable()
export class AuthService {
  public static Token: LoginResponse;
  private _client: Client;

  constructor(client: Client) {
      this._client = client;

      this.hydrate();
  }

  public static isLoggedIn() {
    return AuthService.Token != null && new Date(AuthService.Token.expiration).getTime() > new Date(Date.now()).getTime();
  }

  loggedIn(): any {
    return AuthService.isLoggedIn();
  }

  private hydrate() {
    const storedToken = localStorage.getItem('token');
    if (this.storeToken !== null) {
      AuthService.Token = JSON.parse(storedToken);
    }
  }

  public async login(email: string, password: string): Promise<OperationResultOfLoginResponse> {
    return new Promise<OperationResultOfLoginResponse>((resolve, reject) => {
      this._client.apiAccountLoginPost(new LoginRequestModel({
        email: email,
        password: password
      })).then(async result => {
        if (result.isSuccess) {
          AuthService.Token = result.data;
          this.storeToken(result.data);
          const userResult = await this._client.apiAccountInfoGet();
          this.storeUserProfile(userResult.data);
          resolve(userResult);
        } else {
          reject();
        }
      }).catch(err => reject(err));
    });
  }

  public logout() {
    AuthService.Token = null;
    localStorage.removeItem('user');
    localStorage.removeItem('token');
  }

  public getUser(): UserInfoResponseModel {
    return JSON.parse(localStorage.getItem('user'));
  }

  private storeUserProfile(userModel: UserInfoResponseModel) {
    localStorage.setItem('user', JSON.stringify(userModel));
  }

  private storeToken(token: LoginResponse) {
    localStorage.setItem('token', JSON.stringify(token));
  }
}
