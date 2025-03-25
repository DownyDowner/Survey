<template>
  <v-container>
    <v-row>
      <v-col cols="12" md="8">
        <v-autocomplete
          clearable
          label="Search"
          :items="questionStore.activeQuestions"
          hide-details
          item-title="name"
          rounded
          prepend-inner-icon="mdi-magnify"
          v-model:search="searchQuery"
        />
      </v-col>
      <v-col
        v-if="authenticationStore.role === RoleConstants.ADMIN"
        cols="12"
        md="4"
        class="d-flex justify-md-end"
      >
        <v-btn
          text="Add Question"
          class="mt-1"
          prepend-icon="mdi-plus"
          color="warning"
          @click.stop="openCreateQuestionDialog"
        />
      </v-col>
      <v-col cols="12">
        <QuestionsList
          ref="questionList"
          :questions="filteredQuestions"
          :is-closed="false"
          @on-question-deleted="loadActiveQuestions"
        />
      </v-col>
    </v-row>
  </v-container>
  <CreateQuestionDialog
    ref="createQuestionDialog"
    @on-question-added="loadActiveQuestions"
  />
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useQuestionStore } from "../../stores/question";
import QuestionsList from "./components/QuestionsList.vue";
import { useNotificationStore } from "../../stores/notification";
import CreateQuestionDialog from "../admin/components/CreateQuestionDialog.vue";
import { useAuthenticationStore } from "../../stores/authentication";
import { RoleConstants } from "../../Constants/roleConstants";

const questionStore = useQuestionStore();
const notificationStore = useNotificationStore();
const authenticationStore = useAuthenticationStore();

const questionList = ref<InstanceType<typeof QuestionsList> | null>(null);
const searchQuery = ref("");
const createQuestionDialog = ref<InstanceType<
  typeof CreateQuestionDialog
> | null>(null);

const filteredQuestions = computed(() => {
  if (!searchQuery.value) return questionStore.activeQuestions;
  return questionStore.activeQuestions.filter((q) =>
    q.name.toLowerCase().includes(searchQuery.value.toLowerCase())
  );
});

onMounted(async () => {
  await loadActiveQuestions();
});

async function loadActiveQuestions() {
  try {
    await questionStore.getAllActiveQuestions();
  } catch (error) {
    notificationStore.showError(
      "An error occurred while loading the active questions."
    );
  }
}

function openCreateQuestionDialog() {
  createQuestionDialog.value?.open();
}
</script>
