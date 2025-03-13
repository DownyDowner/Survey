import { defineStore } from "pinia";
import { useToast } from "vue-toastification";

export const useNotificationStore = defineStore("notification", () => {
  const toast = useToast();
  const showError = (message: string) => {
    toast.error(message);
  };

  const showSuccess = (message: string) => {
    toast.success(message);
  };

  return { showError, showSuccess };
});
