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
              Register
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
      <v-col cols="12" md="4">
        <v-card class="ma-3 pa-3" rounded="lg">
          <v-card-title class="text-center"> Create a Question </v-card-title>
          <v-card-actions class="justify-center my-2">
            <v-btn
              @click.stop="openCreateQuestionDialog"
              prepend-icon="mdi-pencil-outline"
              color="primary"
              size="large"
              rounded
            >
              Create
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
    <RegisterAdminDialog ref="registerAdminDialog" />
    <CreateQuestionDialog ref="createQuestionDialog" />
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";
import RegisterAdminDialog from "./components/RegisterAdminDialog.vue";
import CreateQuestionDialog from "./components/CreateQuestionDialog.vue";

const registerAdminDialog = ref<InstanceType<
  typeof RegisterAdminDialog
> | null>(null);
const createQuestionDialog = ref<InstanceType<
  typeof CreateQuestionDialog
> | null>(null);

function openRegisterAdminDialog() {
  registerAdminDialog.value?.open();
}

function openCreateQuestionDialog() {
  createQuestionDialog.value?.open();
}
</script>
