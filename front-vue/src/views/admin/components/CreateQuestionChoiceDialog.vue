<template>
  <v-dialog
    v-model="isOpen"
    no-click-animation
    scroll-strategy="close"
    max-width="600"
    max-height="600"
    persistent
  >
    <v-form v-model="isValid" @submit.prevent="addChoice">
      <v-card min-height="200">
        <v-toolbar flat dark color="primary">
          <v-toolbar-title>Add a Choice</v-toolbar-title>
          <v-spacer />
          <v-btn icon dark @click.stop="close">
            <v-icon>mdi-close</v-icon>
          </v-btn>
        </v-toolbar>
        <v-card-text>
          <v-row no-gutters>
            <v-col cols="12">
              <v-text-field
                label="Name"
                rounded="lg"
                :rules="choiceNameRules"
                v-model="choiceName"
              />
            </v-col>
          </v-row>
        </v-card-text>
        <v-card-actions>
          <v-btn
            class="ma-2 px-4"
            color="primary"
            variant="flat"
            size="large"
            @click.stop="close"
          >
            Cancel
          </v-btn>
          <v-btn
            type="submit"
            class="ma-2 px-4"
            color="success"
            variant="flat"
            size="large"
            :disabled="!isValid"
            :loading="isLoading"
          >
            Add
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-form>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { ChoiceSave } from "../../../models/question/ChoiceSave";

const isOpen = ref<boolean>(false);
const isValid = ref<boolean>(false);
const isLoading = ref<boolean>(false);
const choiceName = ref<string>("");

const choiceNameRules = [(v: string) => !!v || "Choice name is required"];

const emit = defineEmits<{
  (e: "onChoiceAdded", model: ChoiceSave): void;
}>();

const open = () => {
  isOpen.value = true;
};

const close = () => {
  isOpen.value = false;
  choiceName.value = "";
};

function addChoice() {
  if (!choiceName.value.trim()) return;

  const choice = new ChoiceSave({
    name: choiceName.value,
  });

  emit("onChoiceAdded", choice);
  close();
}

defineExpose({ open });
</script>
