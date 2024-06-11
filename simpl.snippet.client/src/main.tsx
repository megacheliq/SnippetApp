import React from 'react'
import ReactDOM from 'react-dom/client'
import { RouterProvider } from 'react-router-dom'
import router from '@/router.tsx'
import '@/index.css'
import { ThemeProvider } from "@/components/theme-provider.tsx"
import { ContextProvider } from '@/contexts/ContextProvider'
import { Toaster } from "@/components/ui/sonner"

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ContextProvider>
      <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">        
        <RouterProvider router={router} />
        <Toaster />
      </ThemeProvider>
    </ContextProvider>
  </React.StrictMode>
)