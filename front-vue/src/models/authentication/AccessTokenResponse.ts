export class AccessTokenResponseDTO {
  tokenType: null | string = null;
  accessToken: null | string = null;
  expiresIn: null | number = null;
  refreshToken: null | string = null;
}

export class AccessTokenResponse extends AccessTokenResponseDTO {
  constructor(data?: AccessTokenResponseDTO | AccessTokenResponseDTO | null) {
    super();
    if (data) Object.assign(this, data);
  }
}
