import type { AccessTokenResponse } from "../models/authentication/AccessTokenResponse";
import axios from "axios";

export abstract class AuthApi {
  static API_URL = "https://localhost:7223";

  static async register(email: string, password: string): Promise<void> {
    await axios.post(`${AuthApi.API_URL}/register`, {
      email,
      password,
    });
  }

  static async login(
    email: string,
    password: string
  ): Promise<AccessTokenResponse> {
    const response = await axios.post(`${AuthApi.API_URL}/login`, {
      email,
      password,
    });
    return response.data;
  }

  static async getUser(token: string) {
    const response = await axios.get(`${AuthApi.API_URL}/users/me`, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
    return response.data;
  }
}
