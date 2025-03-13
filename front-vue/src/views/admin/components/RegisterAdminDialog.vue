<template>
  <v-dialog
    v-model="isOpen"
    no-click-animation
    scroll-strategy="close"
    max-width="600"
    max-height="600"
    persistent
  >
    <v-form v-model="isValid" @submit.prevent>
      <v-card min-height="200">
        <v-toolbar flat dark color="primary">
          <v-toolbar-title>Register an Admin</v-toolbar-title>
          <v-spacer />
          <v-btn icon dark @click.stop="close">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text>
          <v-text-field
            class="mb-2"
            label="Email"
            rounded="lg"
            prepend-inner-icon="mdi-account"
            v-model="authModel.email"
            autofocus
            :rules="emailRules"
          />
          <v-text-field
            class="mb-2"
            label="Password"
            rounded="lg"
            prepend-inner-icon="mdi-lock"
            :append-inner-icon="showPassword ? `mdi-eye` : 'mdi-eye-off'"
            :type="showPassword ? 'text' : 'password'"
            v-model="authModel.password"
            :rules="passwordRules"
            @click:append-inner="showPassword = !showPassword"
          />
          <v-text-field
            class="mb-2"
            label="Confirm Password"
            rounded="lg"
            prepend-inner-icon="mdi-lock"
            :append-inner-icon="showConfirmPassword ? `mdi-eye` : 'mdi-eye-off'"
            :type="showConfirmPassword ? 'text' : 'password'"
            v-model="confirmPassword"
            :rules="confirmPasswordRules"
            @click:append-inner="showConfirmPassword = !showConfirmPassword"
          />
        </v-card-text>
        <v-card-actions>
          <v-btn
            class="ma-2 px-4"
            color="primary"
            variant="flat"
            size="large"
            @click.stop="close"
          >
            Cancel
          </v-btn>
          <v-btn
            type="submit"
            class="ma-2 px-4"
            color="success"
            variant="flat"
            size="large"
            :disabled="!isValid"
            @click.stop="registerAdmin"
            :loading="isLoading"
          >
            Register
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-form>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useAuthenticationStore } from "../../../stores/authentication";
import { useNotificationStore } from "../../../stores/notification";

const authStore = useAuthenticationStore();
const notificationStore = useNotificationStore();

const isOpen = ref(false);
const isValid = ref(false);
const authModel = ref({ email: "", password: "" });
const confirmPassword = ref("");
const showPassword = ref(false);
const showConfirmPassword = ref(false);
const isLoading = ref(false);

const emailRules = [
  (v: string) => !!v || "Email is required",
  (v: string) => /.+@.+\..+/.test(v) || "E-mail must be valid",
];

const passwordRules = [(v: string) => !!v || "Password is required"];

const confirmPasswordRules = [
  (v: string) => !!v || "Confirm Password is required",
  (v: string) => v === authModel.value.password || "Passwords must match",
];

const open = () => {
  isOpen.value = true;
};

const close = () => {
  isOpen.value = false;
  authModel.value.email = "";
  authModel.value.password = "";
  confirmPassword.value = "";
};

async function registerAdmin() {
  try {
    await authStore.registerAdmin(
      authModel.value.email,
      authModel.value.password
    );
    notificationStore.showSuccess(
      `The admin account for ${authModel.value.email} has been successfully registered.`
    );
    close();
  } catch (error) {
    notificationStore.showError(
      "An error occurred while registering the admin. Please try again."
    );
  }
}

defineExpose({ open });
</script>
