import React, { createContext, useContext, useState, ReactNode } from 'react';
import { Snippet } from '@/models/Snippet';

interface SnippetContextProps {
    currentCode: string | null;
    mainSnippet: Snippet | null;
    expirationDate: Date | null;
    setExpirationDate: (date: Date) => void;
    setMainSnippet: (snippet: Snippet) => void;
    setCurrentCode: (currentCode: string) => void;
}

const SnippetContext = createContext<SnippetContextProps | undefined>(undefined);

export const useSnippetContext = () => {
    const context = useContext(SnippetContext);
    if (!context) {
        throw new Error('useSnippetContext must be used within a SnippetProvider');
    }
    return context;
};

export const SnippetProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [mainSnippet, setMainSnippet] = useState<Snippet | null>(null);
    const [currentCode, setCurrentCode] = useState<string | null>(null);
    const [expirationDate, setExpirationDate] = useState<Date | null>(null);

    return (
        <SnippetContext.Provider value={{ mainSnippet, setMainSnippet, currentCode, setCurrentCode, expirationDate, setExpirationDate }}>
            {children}
        </SnippetContext.Provider>
    );
};