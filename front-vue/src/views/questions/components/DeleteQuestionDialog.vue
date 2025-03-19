<template>
  <v-dialog
    v-model="isOpen"
    no-click-animation
    scroll-strategy="close"
    max-width="600"
    persistent
  >
    <v-card>
      <v-toolbar flat dark color="error">
        <v-toolbar-title>Delete Question</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-btn icon dark @click.stop="close">
          <v-icon>mdi-close</v-icon>
        </v-btn>
      </v-toolbar>
      <v-card-text>
        <span>
          Are you sure you want to delete the following question:
          <b>{{ question?.name }}</b>
        </span>
      </v-card-text>
      <v-card-actions>
        <v-spacer />
        <v-tooltip text="Cancel" location="top">
          <template v-slot:activator="{ props }">
            <v-btn
              v-bind="props"
              class="ma-2 px-4"
              color="primary"
              variant="flat"
              size="large"
              @click.stop="close"
            >
              <v-icon left class="mr-2">mdi-close</v-icon>
              Cancel
            </v-btn>
          </template>
        </v-tooltip>
        <v-tooltip text="Delete the question" location="top">
          <template v-slot:activator="{ props }">
            <v-btn
              v-bind="props"
              class="ma-2 px-4"
              color="error"
              variant="flat"
              size="large"
              :disabled="!question"
              @click.stop="deleteQuestion"
              :loading="isLoading"
            >
              <v-icon left class="mr-2">mdi-content-save</v-icon>
              Delete
            </v-btn>
          </template>
        </v-tooltip>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { Ref, ref } from "vue";
import { QuestionList } from "../../../models/question/QuestionList";
import { useQuestionStore } from "../../../stores/question";
import { useNotificationStore } from "../../../stores/notification";

const questionStore = useQuestionStore();
const notificationStore = useNotificationStore();

const isOpen = ref(false);
const isLoading = ref(false);
const question: Ref<QuestionList | null> = ref(null);

const emit = defineEmits<{
  (e: "onQuestionDeleted"): void;
}>();

const open = (questionToDelete: QuestionList) => {
  question.value = questionToDelete;
  isOpen.value = true;
};

const close = () => {
  isOpen.value = false;
  question.value = null;
};

async function deleteQuestion() {
  try {
    isLoading.value = true;
    if (!question.value) throw new Error();
    await questionStore.deleteQuestion(question.value.id);
    emit("onQuestionDeleted");
    notificationStore.showSuccess("Question successfully deleted.");
    close();
  } catch (error) {
    notificationStore.showError("The question could not be deleted.");
  } finally {
    isLoading.value = false;
  }
}

defineExpose({
  open,
});
</script>
