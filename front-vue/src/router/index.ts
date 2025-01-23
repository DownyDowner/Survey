import {
  type RouteLocationNormalized,
  type RouteRecordRaw,
  createRouter,
  createWebHistory,
} from "vue-router";
import { useAuthenticationStore } from "../stores/authentication";
import { NavigationConst } from "./NavigationConst";
import LayoutConnected from "../layouts/LayoutConnected.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "",
    name: NavigationConst.nameLogin,
    component: () => import("../views/authentication/Login.vue"),
    beforeEnter: checkIsNotAuthenticated,
    meta: { title: NavigationConst.titleLogin },
  },
  {
    path: NavigationConst.routeRegister,
    name: NavigationConst.nameRegister,
    component: () => import("../views/authentication/Register.vue"),
    beforeEnter: checkIsNotAuthenticated,
    meta: { title: NavigationConst.titleRegister },
  },
  {
    path: "",
    component: LayoutConnected,
    beforeEnter: checkIsAuthenticated,
    children: [
      {
        path: NavigationConst.routeHome,
        name: NavigationConst.nameHome,
        component: () => import("../views/questions/ActiveQuestions.vue"),
        meta: { title: NavigationConst.titleHome },
      },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
});

router.afterEach((to: RouteLocationNormalized) => {
  document.title = `${to.meta?.title ?? ""} · ${NavigationConst.nameApp}`;
});

async function checkIsNotAuthenticated() {
  const authStore = useAuthenticationStore();
  await authStore.loadUser();
  // if (authStore.token) return { name: NavigationConst.Home };
}

async function checkIsAuthenticated(to: RouteLocationNormalized) {
  const authStore = useAuthenticationStore();
  await authStore.loadUser();
  if (!authStore.token)
    return { name: NavigationConst.nameLogin, query: { redirect: to.path } };
}

export default router;
