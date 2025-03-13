<template>
  <v-container>
    <v-row class="mx-1" no-gutters justify="center">
      <v-col cols="12" md="4">
        <v-card class="ma-3 pa-3" rounded="lg">
          <v-card-title class="text-center"> Register an Admin </v-card-title>
          <v-card-actions class="justify-center my-2">
            <v-btn
              @click.stop="openRegisterAdminDialog"
              prepend-icon="mdi-plus"
              color="primary"
              size="large"
              rounded
            >
              Add
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    <RegisterAdminDialog ref="registerAdminDialog" />
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";
import RegisterAdminDialog from "./components/RegisterAdminDialog.vue";

const registerAdminDialog = ref<InstanceType<
  typeof RegisterAdminDialog
> | null>(null);

function openRegisterAdminDialog() {
  registerAdminDialog.value?.open();
}
</script>
