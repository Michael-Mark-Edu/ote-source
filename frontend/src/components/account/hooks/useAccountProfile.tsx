import { useCallback, useEffect, useState } from "react";
import type { UserGetDto, UserPatchDto } from "../../../api/users";
import { getUserById, patchUser } from "../../../api/users";

type SaveState = "idle" | "saving" | "saved" | "error";

type FormState = {
  username: string;
  firstName: string;
  middleName: string;
  lastName: string;
};

export function useAccountProfile(userId: number | null) {
  const [profile, setProfile] = useState<UserGetDto | null>(null);
  const [loading, setLoading] = useState(true);
  const [form, setForm] = useState<FormState>({
    username: "",
    firstName: "",
    middleName: "",
    lastName: "",
  });

  const [saveState, setSaveState] = useState<SaveState>("idle");
  const [errorMsg, setErrorMsg] = useState<string | null>(null);
  
  const reload = useCallback(async () => {
    if (!userId) return;

    setLoading(true);
    setErrorMsg(null);

    try {
      const user = await getUserById(userId);
      setProfile(user);
      setForm({
        username: user.username ?? "",
        firstName: user.firstName ?? "",
        middleName: user.middleName ?? "",
        lastName: user.lastName ?? "",
      });
    } catch (err) {
      setErrorMsg(err instanceof Error ? err.message : "Failed to load profile.");
    } finally {
      setLoading(false);
    }
  }, [userId]);

  useEffect(() => {
    if (userId) {
      reload();
    } else {
      setProfile(null);
      setErrorMsg(null);
      setSaveState("idle");
    }
  }, [userId, reload]);

  function setField<K extends keyof FormState>(key: K, value: FormState[K]) {
    setForm((prev) => ({ ...prev, [key]: value }));
  }

  function validate(): string | null {
    const u = form.username.trim();
    if (!u) return "Username is required.";
    if (u.length > 255) return "Username must be less than 255 characters.";
    if (form.firstName.trim().length > 255) return "First name must be less than 255 characters.";
    if (form.middleName.trim().length > 255) return "Middle name must be less than 255 characters.";
    if (form.lastName.trim().length > 255) return "Last name must be less than 255 characters.";
    return null;
  }

  async function save() {
    setErrorMsg(null);

    if (!userId) {
      setSaveState("error");
      setErrorMsg("Not logged in.");
      return;
    }

    if (!profile) {
      setSaveState("error");
      setErrorMsg("Profile not loaded.");
      return;
    }

    const validationError = validate();
    if (validationError) {
      setSaveState("error");
      setErrorMsg(validationError);
      return;
    }
    setSaveState("saving");

    const patch: UserPatchDto = {
      username: form.username.trim(),
      firstName: form.firstName.trim() || null,
      middleName: form.middleName.trim() || null,
      lastName: form.lastName.trim() || null,
    };

    try {
      const updated = await patchUser(userId, patch);
      setProfile(updated);
      setSaveState("saved");
      setTimeout(() => setSaveState("idle"), 1500);
    } catch (err) {
      setSaveState("error");
      setErrorMsg(err instanceof Error ? err.message : "Failed to save.");
    }
  }

  return { profile, form, setField, save, saveState, errorMsg, loading, reload };
}