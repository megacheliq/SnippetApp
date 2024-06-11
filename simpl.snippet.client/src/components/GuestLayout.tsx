import { Navigate, Outlet } from "react-router-dom";
import { useStateContext } from '@/contexts/ContextProvider'
import { ModeToggle } from '@/components/ui/mode-toggle'

export default function GuestLayout() {
    const {token} = useStateContext();
    if (token) {
        return <Navigate to='/'/>
    }
    
    return (
        <>
            <div className="absolute bottom-0 right-0 m-4 z-10"><ModeToggle /></div>
            <Outlet/>
        </>
    )
}