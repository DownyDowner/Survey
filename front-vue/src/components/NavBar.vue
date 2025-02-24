<template>
  <v-navigation-drawer
    v-model="drawer"
    app
    temporary
    hide-overlay
    class="d-md-none d-flex"
  >
    <v-list>
      <v-list-item @click="home">
        <v-list-item-icon>
          <v-icon>mdi-home</v-icon>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title>Home</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-list-item>
        <v-list-item @click="closedQuestions">
          <v-list-item-icon>
            <v-icon>mdi-chart-bar</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>Closed Questions</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        <v-list-item></v-list-item>
        <v-list-item-content>
          <v-list-item-title>{{ email }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-list-item v-if="authStore.token" @click="logout">
        <v-list-item-icon>
          <v-icon>mdi-logout</v-icon>
        </v-list-item-icon>
        <v-list-item-content>
          <v-list-item-title>Déconnexion</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>
  </v-navigation-drawer>

  <v-app-bar flat color="primary" density="compact">
    <v-row class="w-100 no-gutters mx-2">
      <v-app-bar-nav-icon @click="drawer = !drawer" class="d-md-none" />
      <v-col cols="auto">
        <v-app-bar-title>
          {{ NavigationConst.nameApp }} · {{ currentTitle }}
        </v-app-bar-title>
      </v-col>
      <v-col cols="auto" class="d-none d-md-flex">
        <v-btn @click.stop="home" class="d-flex align-center">
          <v-icon left>mdi-home</v-icon>
          <span>Home</span>
        </v-btn>
      </v-col>
      <v-col cols="auto" class="d-none d-md-flex">
        <v-btn @click.stop="closedQuestions" class="d-flex align-center">
          <v-icon left>mdi-chart-bar</v-icon>
          <span>Closed Questions</span>
        </v-btn>
      </v-col>
      <v-spacer></v-spacer>
      <v-col cols="auto" class="d-none d-md-flex">
        <v-list-item class="d-flex align-center">
          <v-list-item-subtitle>{{ email }}</v-list-item-subtitle>
        </v-list-item>
      </v-col>
      <v-col cols="auto" class="d-none d-md-flex">
        <v-btn
          v-if="authStore.token"
          icon
          color="white"
          size="small"
          variant="flat"
          @click.stop="logout"
        >
          <v-icon color="primary">mdi-logout</v-icon>
        </v-btn>
      </v-col>
    </v-row>
  </v-app-bar>
</template>

<script setup lang="ts">
import { computed, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { onMounted, watch } from "vue";
import { useAuthenticationStore } from "../stores/authentication";
import { NavigationConst } from "../router/NavigationConst";

const authStore = useAuthenticationStore();
const router = useRouter();
const currentRoute = useRoute();

const currentTitle = ref("");
const drawer = ref(false);

const email = computed(() => authStore.email);

onMounted(() => {
  displayRoute();
});

watch(currentRoute, () => displayRoute());

function displayRoute() {
  currentTitle.value = currentRoute.meta?.title as string;
}

function home() {
  router.push({ name: NavigationConst.nameHome });
}

function closedQuestions() {
  router.push({ name: NavigationConst.nameClosed });
}

async function logout() {
  await authStore.logout();
  router.replace({ name: NavigationConst.nameLogin });
}
</script>
