<template>
  <v-container>
    <v-row>
      <v-col cols="12" md="4">
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
      <v-col cols="12">
        <QuestionsList
          ref="questionList"
          :questions="filteredQuestions"
          :is-closed="false"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useQuestionStore } from "../../stores/question";
import QuestionsList from "./components/QuestionsList.vue";
import { useNotificationStore } from "../../stores/notification";

const questionStore = useQuestionStore();
const notificationStore = useNotificationStore();

const questionList = ref<InstanceType<typeof QuestionsList> | null>(null);
const searchQuery = ref("");

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
</script>
