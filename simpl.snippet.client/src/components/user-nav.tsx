import {
    Avatar,
    AvatarFallback,
    AvatarImage,
} from "@/components/ui/avatar"
import { Button } from "@/components/ui/button"
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { useStateContext } from "@/contexts/ContextProvider"
import { useTheme } from "@/components/theme-provider"

export function UserNav() {
    const { user, logout } = useStateContext();
    const { theme, setTheme } = useTheme();

    const onLogout = (e: React.MouseEvent) => {
        e.preventDefault();
        logout();
    }

    const toggleTheme = () => {
        setTheme(theme === "dark" ? "light" : "dark");
    };

    return (
        <DropdownMenu>
            <DropdownMenuTrigger asChild>
                <Button variant="ghost" className="relative h-8 w-8 rounded-full">
                    <Avatar className="h-8 w-8">
                        <AvatarImage src="" alt='Аватар' />
                        <AvatarFallback>{user?.email?.charAt(0).toUpperCase()}</AvatarFallback>
                    </Avatar>
                </Button>
            </DropdownMenuTrigger>
            <DropdownMenuContent className="w-56" align="end" forceMount>
                <DropdownMenuLabel className="font-normal">
                    <div className="flex flex-col space-y-1">
                        <p className="font-medium leading-none">{user?.email ?? 'Аноним'}</p>
                        <p className="leading-none text-muted-foreground">
                            {user?.name ?? 'User'}
                        </p>
                    </div>
                </DropdownMenuLabel>
                <DropdownMenuItem onClick={toggleTheme} className="cursor-pointer">
                    Смена темы
                </DropdownMenuItem>
                <DropdownMenuSeparator />
                {user && (
                    <DropdownMenuItem onClick={onLogout} className="cursor-pointer">
                        Выход
                    </DropdownMenuItem>
                )}
            </DropdownMenuContent>
        </DropdownMenu>
    )
}