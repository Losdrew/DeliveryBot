import React from 'react'
import ReactDOM from 'react-dom/client'
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import App from './App.tsx'
import { AuthProvider } from './context/AuthProvider'
import { CartProvider } from './context/CartProvider.tsx'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <BrowserRouter>
      <CartProvider>
        <AuthProvider>
          <Routes>
            <Route path="/*" element={<App />} />
          </Routes>
        </AuthProvider>
      </CartProvider>
    </BrowserRouter>
  </React.StrictMode>
)
