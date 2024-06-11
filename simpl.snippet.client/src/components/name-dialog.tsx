import {
    AlertDialog,
    AlertDialogContent
} from "@/components/ui/alert-dialog"
import { useState, useEffect } from "react"
import NameForm from "@/components/forms/name-form";

interface NameDialogProps {
    setSubmitted: React.Dispatch<React.SetStateAction<boolean>>
}

const NameDialog: React.FC<NameDialogProps> = ({setSubmitted}) => {
    const [sessionId, setSessionId] = useState('');
    const [isOpen, setIsOpen] = useState<boolean>(true);

    useEffect(() => {
        setSessionId(localStorage.getItem('sessionId') || '');
    }, []);
    
    useEffect(() => {
        if (sessionId) {
            setIsOpen(!(document.cookie.includes(`registrated_on_${sessionId}=true`)));
            setSubmitted(document.cookie.includes(`registrated_on_${sessionId}=true`));
        }
    }, [sessionId]);

    return (
        <>
            <AlertDialog open={isOpen}>
                <AlertDialogContent>
                    <NameForm setIsOpen={setIsOpen} setSubmitted={setSubmitted}/>
                </AlertDialogContent>
            </AlertDialog>
        </>
    );
}

export default NameDialog;
