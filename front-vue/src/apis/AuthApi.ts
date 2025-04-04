import axios from "axios";
import { useAuthenticationStore } from "../stores/authentication";

export abstract class AuthApi {
  static API_URL = "https://localhost:7223/api/auth";

  static async register(email: string, password: string): Promise<void> {
    await axios.post(`${AuthApi.API_URL}/register`, {
      email,
      password,
    });
  }

  static async registerAdmin(email: string, password: string): Promise<void> {
    const authStore = useAuthenticationStore();
    await axios.post(
      `${AuthApi.API_URL}/register-admin`,
      {
        email,
        password,
      },
      {
        headers: { Authorization: "Bearer " + authStore.token },
        responseType: "json",
      }
    );
  }

  static async login(email: string, password: string): Promise<string> {
    const response = await axios.post(`${AuthApi.API_URL}/login`, {
      email,
      password,
    });
    return response.data;
  }

  static async logout(): Promise<void> {
    const token = localStorage.getItem("Auth-Token");
    if (!token) return;

    try {
      await axios.post(
        `${AuthApi.API_URL}/logout`,
        {},
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );
      localStorage.removeItem("Auth-Token");
    } catch (error) {
      throw error;
    }
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
