import { defineStore } from "pinia";
import { useToast } from "vue-toastification";

export const useNotificationStore = defineStore("notification", () => {
  const toast = useToast();
  const showError = (message: string) => {
    toast.error(message);
  };

  return { showError };
});
