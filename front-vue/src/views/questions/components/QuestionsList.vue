<template>
  <v-list v-if="props.questions.length > 0">
    <v-list-item
      v-for="question in props.questions"
      :key="question.id"
      :title="question.name"
      :subtitle="formatDateRange(question.beginDate, question.endDate)"
    >
      <template v-slot:append>
        <v-btn
          color="primary"
          icon="mdi-chevron-right"
          variant="text"
          @click.stop="openDetailsQuestion(question)"
        />
        <v-btn
          v-if="authenticationStore.role === RoleConstants.ADMIN"
          color="error"
          icon="mdi-delete"
          density="compact"
          @click.stop="openDeleteQuestionDialog(question)"
        />
      </template>
    </v-list-item>
  </v-list>
  <v-alert v-else type="info" variant="outlined">
    No questions available at this time.
  </v-alert>
  <DeleteQuestionDialog
    ref="deleteQuestionDialog"
    @on-question-deleted="onQuestionDeleted"
  />
</template>

<script setup lang="ts">
import { ref } from "vue";
import { RoleConstants } from "../../../Constants/roleConstants";
import { QuestionList } from "../../../models/question/QuestionList";
import router from "../../../router";
import { NavigationConst } from "../../../router/NavigationConst";
import { useAuthenticationStore } from "../../../stores/authentication";
import DeleteQuestionDialog from "./DeleteQuestionDialog.vue";

const props = defineProps<{
  questions: QuestionList[];
  isClosed: boolean;
}>();

const emit = defineEmits<{
  (e: "onQuestionDeleted"): void;
}>();

const authenticationStore = useAuthenticationStore();

const deleteQuestionDialog = ref<InstanceType<
  typeof DeleteQuestionDialog
> | null>(null);

function formatDateRange(beginDate: string, endDate: string): string {
  const begin = new Date(beginDate).toLocaleDateString();
  const end = new Date(endDate).toLocaleDateString();

  return `From ${begin} to ${end}`;
}

function openDetailsQuestion(question: QuestionList) {
  if (props.isClosed) {
    router.push({
      name: NavigationConst.nameClosedDetails,
      params: { id: question.id },
    });
  }
}

function openDeleteQuestionDialog(question: QuestionList) {
  deleteQuestionDialog.value?.open(question);
}

function onQuestionDeleted() {
  emit("onQuestionDeleted");
}
</script>
