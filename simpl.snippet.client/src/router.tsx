import { createBrowserRouter, Navigate } from "react-router-dom";
import Hub from "@/components/pages/Hub";
import CodePage from "@/components/pages/CodePage";
import NotFound from "@/components/pages/NotFound";
import DefaultLayout from "./components/DefaultLayout";
import GuestLayout from "./components/GuestLayout";
import Login from "./components/pages/LoginPage";
import { SnippetProvider } from "@/contexts/SnippetContext";
import { DialogProvider } from "@/contexts/DialogContext";

const router = createBrowserRouter([
    {
        path: '/',
        element: <DefaultLayout />,
        children: [
            {
                path: '/',
                element: <DialogProvider><Hub /></DialogProvider>
            },
            
        ]
    },
    {
        path: '/',
        element: <GuestLayout />,
        children: [
            {
                path: '/login',
                element: <Login />
            }
        ]
    },
    {
        path: '/code/:sessionId',
        element: <SnippetProvider><CodePage /></SnippetProvider>
    },
    {
        path: '/404',
        element: <NotFound />
    },
    {
        path: '*',
        element: <Navigate to='/404' />
    },
])

export default router;