<template>
  <v-container>
    <v-row>
      <v-col cols="12" md="4">
        <v-autocomplete
          clearable
          label="Search by name"
          :items="questionStore.activeQuestions"
          hide-details
          item-title="name"
          rounded
          prepend-inner-icon="mdi-magnify"
          v-model:search="searchQuery"
        />
      </v-col>
      <v-col cols="12" md="3">
        <v-text-field
          label="Start Date"
          type="date"
          v-model="startDateFilter"
          clearable
          hide-details
        />
      </v-col>
      <v-col cols="12" md="3">
        <v-text-field
          label="End Date"
          type="date"
          v-model="endDateFilter"
          clearable
          hide-details
        />
      </v-col>
      <v-col
        v-if="authenticationStore.role === RoleConstants.ADMIN"
        cols="12"
        md="2"
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
const startDateFilter = ref<string | null>(null);
const endDateFilter = ref<string | null>(null);
const createQuestionDialog = ref<InstanceType<
  typeof CreateQuestionDialog
> | null>(null);

const filteredQuestions = computed(() => {
  return questionStore.activeQuestions.filter((q) => {
    const matchesSearch = searchQuery.value
      ? q.name.toLowerCase().includes(searchQuery.value.toLowerCase())
      : true;

    const matchesStartDate = startDateFilter.value
      ? new Date(q.beginDate) >= new Date(startDateFilter.value)
      : true;

    const matchesEndDate = endDateFilter.value
      ? new Date(q.endDate) <= new Date(endDateFilter.value)
      : true;

    return matchesSearch && matchesStartDate && matchesEndDate;
  });
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
