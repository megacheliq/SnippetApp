import React from 'react';
import LoginForm from '@/components/forms/login-form';
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

const Login: React.FC = () => {
  return (
    <>
      <div className="flex items-center justify-center h-screen w-full">
        <Card className='w-1/4'>
          <CardHeader>
            <CardTitle>Авторизация</CardTitle>
            <CardDescription>Войти в существующий аккаунт</CardDescription>
          </CardHeader>
          <CardContent>
            <LoginForm />
          </CardContent>
        </Card>
      </div>
    </>
  );
};

export default Login;