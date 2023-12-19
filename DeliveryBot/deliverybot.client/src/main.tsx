import React from 'react'
import ReactDOM from 'react-dom/client'
import { I18nextProvider } from 'react-i18next'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import App from './App.tsx'
import { AuthProvider } from './context/AuthProvider'
import { CartProvider } from './context/CartProvider.tsx'
import i18n from './i18n'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <I18nextProvider i18n={i18n}>
      <BrowserRouter>
        <CartProvider>
          <AuthProvider>
            <Routes>
              <Route path="/*" element={<App />} />
            </Routes>
          </AuthProvider>
        </CartProvider>
      </BrowserRouter>
    </I18nextProvider>
  </React.StrictMode>
)
