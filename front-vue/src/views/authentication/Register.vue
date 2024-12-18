<template>
  <v-container
    fluid
    class="align-start border-app"
    style="max-width: 500px; max-height: 500px"
  >
    <v-card rounded="lg" style="max-width: 400px" class="mx-auto my-auto">
      <v-card-title style="word-break: keep-all">
        <h2 class="text-primary text-center">Enregistrement</h2>
        <h4 class="text-center mt-2">Veuillez entrer vos identifiants</h4>
      </v-card-title>
      <v-card-text class="mt-5">
        <v-form v-model="isValid">
          <v-text-field
            class="mb-2"
            label="Email"
            rounded="lg"
            prepend-inner-icon="mdi-account"
            v-model="authModel.email"
            autofocus
            :rules="stringRules"
          />
          <v-text-field
            class="mb-2"
            label="Mot de passe"
            rounded="lg"
            prepend-inner-icon="mdi-lock"
            :append-inner-icon="showPassword ? `mdi-eye` : 'mdi-eye-off'"
            :type="showPassword ? 'text' : 'password'"
            v-model="authModel.password"
            :rules="stringRules"
            @click:append-inner="showPassword = !showPassword"
          />
          <v-text-field
            class="mb-2"
            label="Confirmation du mot de passe"
            rounded="lg"
            prepend-inner-icon="mdi-lock"
            :append-inner-icon="showConfirmPassword ? `mdi-eye` : 'mdi-eye-off'"
            :type="showConfirmPassword ? 'text' : 'password'"
            v-model="confirmPassword"
            :rules="stringRules"
            @keypress.enter="signUp"
            @click:append-inner="showConfirmPassword = !showConfirmPassword"
          />
          <v-alert v-if="passwordsMismatch" type="error" outlined class="mb-2">
            Les mots de passe ne correspondent pas.
          </v-alert>
        </v-form>

        <v-btn color="primary" block :disabled="!isValid" @click.stop="signUp">
          S'enregistrer
        </v-btn>
        <v-btn
          @click.stop="backToLogin"
          color="grey-lighten-3 mt-1"
          variant="flat"
          block
        >
          Annuler
        </v-btn>
      </v-card-text>
      <v-alert
        v-if="authModel.errorMessages"
        type="error"
        class="mt-2"
        dismissible
      >
        {{ authModel.errorMessages }}
      </v-alert>
    </v-card>
  </v-container>
</template>

<script setup lang="ts">
import { reactive, ref } from "vue";
import router from "../../router";
import { useAuthenticationStore } from "../../stores/authentication";
import { NavigationConst } from "../../router/NavigationConst";

const authStore = useAuthenticationStore();

const stringRules = ref<any[]>([(v: string) => !!v || "Valeur obligatoire"]);
const showPassword = ref<boolean>(false);
const showConfirmPassword = ref<boolean>(false);
const isValid = ref<boolean>(true);
const confirmPassword = ref<string>("");
const passwordsMismatch = ref<boolean>(false);

const authModel = reactive({
  email: "",
  password: "",
  errorMessages: "",
});

async function signUp() {
  try {
    passwordsMismatch.value = authModel.password !== confirmPassword.value;
    if (!passwordsMismatch.value) {
      await authStore.register(authModel.email, authModel.password);
      router.push({ name: NavigationConst.nameLogin });
    } else {
      authModel.password = "";
      confirmPassword.value = "";
    }
  } catch (err) {
    authModel.errorMessages = "Échec de l'enregistrement. Veuillez réessayer";
  }
}

function backToLogin() {
  router.push({ name: NavigationConst.nameLogin });
}
</script>
