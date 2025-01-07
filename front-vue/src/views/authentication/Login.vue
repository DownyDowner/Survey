<template>
  <v-container fluid class="d-flex align-center justify-center">
    <v-card rounded="lg" style="width: 400px">
      <v-card-title class="text-center">
        <h2 class="text-primary">Connexion</h2>
        <h4 class="mt-2">Veuillez entrer vos identifiants</h4>
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
            @keypress.enter="login"
            @click:append-inner="showPassword = !showPassword"
          />
        </v-form>

        <v-btn color="primary" block :disabled="!isValid" @click.stop="login">
          Connexion
        </v-btn>
        <v-btn
          color="grey-lighten-3 mt-1"
          variant="flat"
          block
          @click.stop="createAccount"
        >
          Créer un compte
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
import { useAuthenticationStore } from "../../stores/authentication";
import router from "../../router";
import { NavigationConst } from "../../router/NavigationConst";

const authStore = useAuthenticationStore();

const stringRules = ref<any[]>([(v: string) => !!v || "Valeur obligatoire"]);
const showPassword = ref<boolean>(false);
const isValid = ref<boolean>(true);

const authModel = reactive({
  email: "",
  password: "",
  errorMessages: "",
});

async function login() {
  try {
    await authStore.login(authModel.email, authModel.password);
    router.push({ name: NavigationConst.nameHome });
  } catch (err) {
    authModel.errorMessages =
      "Échec de la connexion. Veuillez vérifier vos identifiants et réessayer";
    authModel.password = "";
  }
}

function createAccount() {
  router.push({ name: NavigationConst.nameRegister });
}
</script>
