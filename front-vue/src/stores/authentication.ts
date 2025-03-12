import { jwtDecode } from "jwt-decode";
import { AuthApi } from "../apis/AuthApi";
import { defineStore } from "pinia";
import { computed, ref, type Ref } from "vue";

export const useAuthenticationStore = defineStore("authentication", () => {
  const token: Ref<string> = ref(localStorage.getItem("Auth-Token") || "");
  const user = ref<any | null>(null);
  const email = computed(() => user.value?.email);
  const isLoading = ref(false);

  const role = computed(() => {
    if (!token.value) return null;
    try {
      const decoded: any = jwtDecode(token.value);
      return (
        decoded[
          "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        ] || null
      );
    } catch (error) {
      return null;
    }
  });

  async function register(email: string, password: string): Promise<void> {
    try {
      isLoading.value = true;
      await AuthApi.register(email, password);
    } catch (error) {
      throw error;
    } finally {
      isLoading.value = false;
    }
  }

  async function login(email: string, password: string): Promise<void> {
    try {
      isLoading.value = true;
      const response = await AuthApi.login(email, password);
      if (response) {
        token.value = response;
        localStorage.setItem("Auth-Token", token.value);
      }
    } catch (error) {
      throw error;
    } finally {
      isLoading.value = false;
    }
  }

  async function logout(): Promise<void> {
    try {
      isLoading.value = true;
      await AuthApi.logout();
      resetToken();
    } catch (error) {
      throw error;
    } finally {
      isLoading.value = false;
    }
  }

  async function loadUser(): Promise<void> {
    if (!token.value) {
      user.value = null;
      return;
    }
    try {
      const userData = await AuthApi.getUser(token.value);
      user.value = userData;
    } catch (error) {
      resetToken();
    }
  }

  function resetToken(): void {
    token.value = "";
    user.value = null;
  }

  return {
    isLoading,
    token,
    user,
    email,
    role,
    register,
    login,
    logout,
    loadUser,
  };
});
